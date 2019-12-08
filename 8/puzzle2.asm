org 100h


section .text

start:
        ; set image to all transparent
        mov     al, 255
        mov     cx, size
        push    ds
        pop     es
        mov     di, image
        rep     stosb

        ; open input file
        mov     dx, input
        call    open_file
        jc      error

next_layer:
        ; reset buffer
        xor     al, al
        mov     cx, size
        mov     di, buffer
        rep     stosb

        ; read layer from input file
        mov     cx, size
        mov     dx, buffer
        call    read_from_file
        jc      error

        ; EOF?
        cmp     ax, 0
        je      output_image

        ; process layer
        mov     si, buffer
        mov     di, image
        mov     word [counter1], height
.next_row:
        mov     word [counter2], width
.next_col:
        ; check if layer pixel is transparent
        lodsb
        cmp     al, '2'
        je      .skip_pixel

        ; set pixel "color"
        cmp     al, '0'
        je      .black
        cmp     al, '1'
        je      .white
        jmp     .skip_pixel
.black:
        mov     dl, '.'
        jmp     .set_pixel
.white:
        mov     dl, '#'
        jmp     .set_pixel
.skip_pixel:
        inc     di
        jmp     .loop
.set_pixel:
        ; check if existing image pixel is transparent
        mov     al, [es:di]
        cmp     al, 255
        jne     .skip_pixel

        ; save new image pixel
        mov     al, dl
        stosb
.loop:
        dec     word [counter2]
        jnz     .next_col
        dec     word [counter1]
        jnz     .next_row
        jmp     next_layer

output_image:
        call    close_file
        jc      error

        ; print rendered image buffer
        mov     si, image
        mov     word [counter1], height
        mov     ah, 2
.next_row:
        mov     word [counter2], width
.next_col:
        ; print pixel character
        lodsb
        mov     dl, al
        int     21h
        dec     word [counter2]
        jnz     .next_col

        ; advance to next line
        call    print_newline
        dec     word [counter1]
        jnz     .next_row

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
buffer          resb    size
image           resb    size
counter1        dw      0
counter2        dw      0
