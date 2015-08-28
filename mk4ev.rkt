#lang racket

(provide run run*
         == =/=
         fresh
         conde
         symbolo numbero
         absento
         (all-defined-out))

;; extra stuff for racket
;; due mostly to samth
(define (list-sort f l) (sort l f))

(define (remp f l) (filter-not f l))

(define (call-with-string-output-port f)
  (define p (open-output-string))
  (f p)
  (get-output-string p))

(define (exists f l) (ormap f l))

(define for-all andmap)

(define (find f l)
  (cond [(memf f l) => car] [else #f]))

(define memp memf)

(define (var*? v) (var? (car v)))


; Substitution representation

(define empty-subst-map (hasheq))

(define subst-map-length hash-count)

; Returns #f if not found, or a pair of u and the result of the lookup.
; This distinguishes between #f indicating absence and being the result.
(define subst-map-lookup
  (lambda (u S)
    (hash-ref S u unbound)))

(define (subst-map-add S var val)
  (hash-set S var val))

(define subst-map-eq? eq?)


; Constraint store representation

(define empty-C (hasheq))

(define set-c
  (lambda (v c st)
    (state (state-S st) (hash-set (state-C st) v c))))

(define lookup-c
  (lambda (v st)
    (hash-ref (state-C st) v empty-c)))

(define remove-c
  (lambda (v st)
    (state (state-S st) (hash-remove (state-C st) v))))


(include "mk.scm")

(define appendo
  (λ (l s ls)
    (conde
     [(== '() l) (== ls s)]
     [(fresh (first rest result)
             (== (cons first rest) l)
             (== (cons first result) ls)
             (appendo rest s result)
             )])))

(define evalo
  (lambda (expr env val)
    (conde
      [(numbero expr) (== expr val)]
      [(== #f expr) (== expr val)]
      [(== #t expr) (== expr val)]
      [(fresh (datum)
         (== `(quote ,datum) expr)
         (not-in-envo 'quote env)
         (absento 'closure datum)
         (== datum val))]
      [(fresh (head-e tail-e head-v tail-v)
         (== `(cons ,head-e ,tail-e) expr)
         (== `(,head-v . ,tail-v) val)
         (not-in-envo 'cons env)
         (evalo head-e env head-v)
         (evalo tail-e env tail-v))]
      [(fresh (e tail)
         (== `(car ,e) expr)
         (not-in-envo 'car env)
         (evalo e env `(,val . ,tail)))]
      [(fresh (e head)
         (== `(cdr ,e) expr)
         (not-in-envo 'cdr env)
         (evalo e env `(,head . ,val)))]
      [(fresh (e1 e2 e3 condition)
              (== expr `(if ,e1 ,e2 ,e3))
              (not-in-envo 'if env)
              (evalo e1 env condition)
              (conde
               [(== condition #t) (evalo e2 env val)]
               [(== condition #f) (evalo e3 env val)]))]

      
      [(symbolo expr)
       (lookupo expr env val)]
      [(fresh (x body)
         (== `(lambda (,x) ,body) expr) 
         (== `(closure ,x ,body ,env) val)
         (not-in-envo 'lambda env))]
            [(fresh (e1 e2 x body env^ arg)
         (== `(,e1 ,e2) expr)
         (evalo e1 env
                `(closure ,x ,body ,env^))
         (evalo e2 env arg)
         (evalo body
                `((,x . ,arg) . ,env^) val))])))

(define lookupo
  (lambda (x env val)
    (fresh (y v rest)
      (== `((,y . ,v) . ,rest) env)
      (conde
        [(== x y) (== v val)]
        [(=/= x y)
         (lookupo x rest val)]))))

(define not-in-envo
  (lambda (x env)
    (conde
      [(== '() env)]
      [(fresh (y v rest)
         (== `((,y . ,v) . ,rest) env)
         (=/= x y)
         (not-in-envo x rest))])))

(run* (q) (evalo '() '() q))
(run* (q) (evalo 'y '((y . #t) (x . #f)) q))
(run* (q) (evalo '(quote (a b c)) '() q))
(run* (q) (evalo '(if #t 1 0) '() q))
(run* (q) (evalo '((lambda (x) 5) 3) '() q))

(run* (q) (evalo '(car '(1 2 3)) '() q))
(run* (q) (evalo '(cdr '(1 2 3)) '() q))
