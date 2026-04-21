import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class StringService {
  constructor() {}

  trlowercase(value: string) {
    let newvalue: string = '';
    for (let i = 0; i < value.length; i++) {
      const element = value[i];

      if (element == 'İ') {
        newvalue += 'i';
      } else if (element == 'I') {
        newvalue += 'ı';
      } else {
        newvalue += element.toLocaleLowerCase();
      }
    }

    return newvalue;
  }


  truppercase(value: string) {
    let newvalue: string = '';
    for (let i = 0; i < value.length; i++) {
      const element = value[i];

      if (element == 'i') {
        newvalue += 'İ';
      } else if (element == 'ı') {
        newvalue += 'I';
      } else {
        newvalue += element.toLocaleUpperCase();
      }
    }
    value[0].toLocaleUpperCase();
    return newvalue;
  }
trTitleCase(value: string) {
  if (!value) return '';

  let words = value.trim().split(/\s+/); // tüm fazla boşlukları tek boşluk yapar

  return words
    .map(word => 
      word.charAt(0).toLocaleUpperCase('tr-TR') + 
      this.trlowercase(word.slice(1))
    )
    .join(' ');
}

  enlowercase(value: string) {
    let newvalue: string = '';
    for (let i = 0; i < value.length; i++) {
      const element = value[i];

      if (element == 'İ') {
        newvalue += 'i';
      } else if (element == 'ı') {
        newvalue += 'i';
      } else {
        newvalue += element.toLocaleLowerCase();
      }
    }

    return newvalue;
  }

   enUppercase(value: string) {
    let newvalue: string = '';
    for (let i = 0; i < value.length; i++) {
      const element = value[i];

      if (element == 'İ') {
        newvalue += 'I';
      } else if (element == 'İ') {
        newvalue += 'I';
      } else {
        newvalue += element.toLocaleLowerCase();
      }
    }

    return newvalue;
  }
}
