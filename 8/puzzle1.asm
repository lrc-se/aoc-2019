org 100h


section .text

start:
        ; open input file
        mov     dx, input
        call    open_file
        jc      error

next_layer:
        ; reset buffer
        mov     cx, size
        xor     al, al
        push    ds
        pop     es
        mov     di, buffer
        rep     stosb

        ; read layer from input file
        mov     cx, size
        mov     dx, buffer
        call    read_from_file
        jc      error

        ; EOF?
        cmp     ax, 0
        je      process_layer

        ; count zeroes in layer
        mov     si, buffer
        mov     word [counter1], 0
count_zeroes:
        lodsb
        cmp     al, '0'
        jne     .loop
        inc     word [counter1]
.loop:
        dec     cx
        jnz     count_zeroes

        ; check zero count against saved value
        mov     ax, [counter1]
        mov     dx, [zeroes]
        cmp     ax, dx
        jnb     next_layer

        ; save new best layer and zero count
        mov     [zeroes], ax
        mov     cx, size
        mov     si, buffer
        push    ds
        pop     es
        mov     di, best_layer
        rep     movsb
        jmp     next_layer

process_layer:
        call    close_file
        jc      error

        ; count ones and twos
        mov     word [counter1], 0
        mov     word [counter2], 0
        mov     cx, size
        mov     si, best_layer
.count_digits:
        lodsb
.check_1:
        cmp     al, '1'
        jne     .check_2
        inc     word [counter1]
        jmp     .loop
.check_2:
        cmp     al, '2'
        jne     .loop
        inc     word [counter2]
.loop:
        dec     cx
        jnz     .count_digits

        ; print counts
        mov     dx, result0
        call    print_string
        mov     dx, [zeroes]
        call    print_hexw
        call    print_newline
        mov     dx, result1
        call    print_string
        mov     dx, [counter1]
        call    print_hexw
        call    print_newline
        mov     dx, result2
        call    print_string
        mov     dx, [counter2]
        call    print_hexw
        call    print_newline

        ; calculate and print result
        mov     dx, result_mul
        call    print_string
        mov     ax, [counter1]
        mul     word [counter2]
        mov     dx, ax
        call    print_hexw
        call    print_newline

exit:
        mov     ax, 4c00h
        int     21h

error:
        mov     dx, errmsg
        call    print_line
        mov     ax, 4c01h
        int     21h


%include "io.asm"


section .data

width   equ     25
height  equ     6
size    equ     width*height

input           db      "input.dat",0
errmsg          db      "Error!$"
result0         db      "Lowest number of 0s: $"
result1         db      "Number of 1s: $"
result2         db      "Number of 2s: $"
result_mul      db      "Result: $"
buffer          resb    size
best_layer      resb    size
counter1        dw      0
counter2        dw      0
zeroes          dw      size+1
