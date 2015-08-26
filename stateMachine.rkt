#lang racket

(letrec ((S1 (λ (str)
               (cond
                 [(null? str) #t]
                 [(= (car str) 0)
                  (S2 (cdr str))]
                 [else (S1 (cdr str))])))
         (S2 (λ (str)
               (cond
                 [(null? str) #f]
                 [(= (car str) 0)
                  (S1 (cdr str))]
                 [else (S2 (cdr str))]))))
  (S1 '(0 1 1 1 0 1 0)))
