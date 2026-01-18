import { Injectable } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';

@Injectable({
    providedIn: 'root'
})
export class DateUtils {
    constructor() {

    }

    getDateNowYYYYMMDD(): any {
        var dateNow = new Date();
        var month = (dateNow.getMonth() + 1);
        var day = dateNow.getDate();
        return dateNow.getFullYear() + '-' + (month < 10 ? '0' + month : month) + "-" + (day < 10 ? '0' + day : day);
    }

    getDateNowToObject(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear(), month: (dateNow.getMonth() + 1), day: dateNow.getDate() };
    }

    getLastMontObject(): any {
        var dateNow = new Date();
        return { year: dateNow.getFullYear(), month: (dateNow.getMonth()), day: dateNow.getDate() };
    }

    getObjectDateByDate(date: Date): any {
        return { year: date.getFullYear(), month: (date.getMonth() + 1), day: date.getDate() };
    }

    getObjectDate(date: Date): any {
        return { year: date.getFullYear() };
    }
    convertDateToObject(date: string) {
        let temp = date.split('T')[0].split('-');
        return { year: Number(temp[0]), month: Number(temp[1]), day: Number(temp[2]) };
    }

    convertObjectToDate(object: any) {
        return object.year + "-" + object.month + "-" + object.day;
    }

    convertObjectToDates(object: any) {
        if(object.month >0 && object.month<10 ) 
        {
            return 0+object.day + "/" + 0+object.month + "/" + object.year;
        }
        else
        {
            return object.day + "/" + object.month + "/" + object.year;
        }
    }

    convertObjectToDateView(data: any) {

        let dateArray1 = data.split('T')[0];
        let dateValue1 = dateArray1.split('-');
        let tempDateFromV1 = {
            'day': parseInt(dateValue1[2]),
            'month': parseInt(dateValue1[1]),
            'year': parseInt(dateValue1[0])
        };
        return tempDateFromV1;
    }
    convertObjectToDateViews(data: any) {

        //let dateArray1 = data.split('/')[0];
        let dateValue1 = data.split('/');
        let tempDateFromV1 = {
            'day': parseInt(dateValue1[0]),
            'month': parseInt(dateValue1[1]),
            'year': parseInt(dateValue1[2])
        };
        return tempDateFromV1;
    }
    getNgbDateStructNow(): NgbDateStruct {
        var dateNow = new Date();
        var dateSubtract: NgbDateStruct = {
            year: dateNow.getFullYear()
            , month: dateNow.getMonth() + 1,
            day: dateNow.getDate()
        };
        return dateSubtract;
    }

    getNgbDateStructStartYear(year: number): NgbDateStruct {
        var dateSubtract: NgbDateStruct = {
            year: year
            , month: 1,
            day: 1
        };
        return dateSubtract;
    }
}
