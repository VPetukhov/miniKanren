#lang racket

(define rember
  (λ (e l)
    (cond
      ((null? l)
       '())
      ((equal? e (car l))
       (cdr l))
      (else
       (cons (car l) (rember e (cdr l)))))))

(rember 5'(1 3 5 7 8))