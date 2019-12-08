; IO functions


; Open file for reading.
; Parameters:
;       DS:DX   zero-terminated file name
; Return value:
;       AX=BX   file number
open_file:
        mov     al, 0
        mov     ah, 3dh
        int     21h
        mov     bx, ax
        ret

; Read from an open file.
; Parameters:
;       BX      file number
;       CX      buffer size
;       DS:DX   buffer address
; Returns:
;       AX      number of bytes read
read_from_file:
        mov     ah, 3fh
        int     21h
        ret

; Seek in an open file.
; Parameters:
;       AL      origin of move
;       BX      file number
;       CX:DX   new position as offset from origin
seek_in_file:
        mov     ah, 42h
        int     21h
        ret

; Close an open file.
; Parameters:
;       BX      file number
close_file:
        mov     ah, 3eh
        int     21h
        ret

; Print a byte in hex format.
; Parameters:
;       DL      Byte to print
print_hexb:
        push    ax
        push    dx
        push    dx
        shr     dl, 4
        call    .print_nibble
        pop     dx
        and     dl, 15
        call    .print_nibble
        pop     dx
        pop     ax
        ret
.print_nibble:
        add     dl, 48
        cmp     dl, 57
        jbe     .print
        add     dl, 7
.print:
        mov     ah, 2
        int     21h
        ret

; Print a word in hex format.
; Parameters:
;       DX      Word to print
print_hexw:
        ror     dx, 8
        call    print_hexb
        ror     dx, 8
        call    print_hexb
        ret

; Print a string.
; Parameters:
;       DS:DX      $-terminated string
print_string:
        push    ax
        mov     ah, 9
        int     21h
        pop     ax
        ret

; Print a string and a line break.
; Parameters:
;       DS:DX      $-terminated string
print_line:
        call    print_string
        call    print_newline
        ret

; Print a line break.
print_newline:
        push    dx
        mov     ah, 2
        mov     dl, 13
        int     21h
        mov     dl, 10
        int     21h
        pop     dx
        ret
