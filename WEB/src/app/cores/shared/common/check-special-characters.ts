import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
  })
export class CheckSpecialCharacter {
    checkCode(code:string) {
        //kiểm tra ký tự đặc việt trong Mã
        var index1 = code.indexOf("*");
        var index2 = code.indexOf("{");
        var index3 = code.indexOf("}");
        var index4 = code.indexOf("!");
        var index5 = code.indexOf("^");
        var index6 = code.indexOf("<");
        var index7 = code.indexOf(">");
        var index8 = code.indexOf("?");
        var index9 = code.indexOf("|");
        var index10 = code.indexOf(",");
        var index11 = code.indexOf("_");
        var index12 = code.indexOf(" ");

        var validCode = true;
        if (index1 != -1 || index2 != -1 || index3 != -1 || index4 != -1 || index5 != -1 || index6 != -1 || index7 != -1 || index8 != -1 || index9 != -1
            || index10 != -1 || index11 != -1 || index12 != -1) {
            validCode = false;
        }
        return validCode;
    }
    keyPress(event: any) {
        const pattern = /[0-9]/;
        const inputChar = String.fromCharCode(event.charCode);
    
        if (!pattern.test(inputChar)) {    
            // invalid character, prevent input
            event.preventDefault();
        }
    }
}