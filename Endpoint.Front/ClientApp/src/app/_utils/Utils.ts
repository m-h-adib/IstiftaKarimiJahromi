import { Router } from "@angular/router";

export class Utils {

  constructor(private router: Router) { }

  public static PersianToEnglish(input: string): number {
    const persianNums = '۰۱۲۳۴۵۶۷۸۹';
    const arabicNums = '0123456789';

    return Number(input.split('').map(c => {
      const i = persianNums.indexOf(c);
      if (i >= 0) return arabicNums.charAt(i);
      else return c;
    }).join(''));
  }

  public static transformNonNumber(number: string): string {
    const persianNumbers = ['۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹'];

    for (var i = 0; i < 10; i++) {
      let reg = new RegExp(persianNumbers[i], "g");
      number = number.replace(reg, i.toString());
    }

    return number;
  }

}
