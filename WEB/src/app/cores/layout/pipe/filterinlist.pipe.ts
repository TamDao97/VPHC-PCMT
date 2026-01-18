import { Pipe, PipeTransform } from '@angular/core';


@Pipe({
  name: 'filterinlist'
})
export class FilterInListPipe implements PipeTransform{
  transform(value: any, listargs: any[]): string {
    let name = '';
    listargs.forEach(item => {
      if (item.id == value) {
        name = item.name;
      }
    });

    return name;
  }
}
