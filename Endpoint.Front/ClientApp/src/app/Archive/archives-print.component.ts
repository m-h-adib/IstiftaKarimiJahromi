import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { GetQuestionPrintService } from "../_services/archive/getQuestionPrintService";
import { GetQuestionPrintDto } from "../_models/archive/getQuestionPrintDto";

@Component({
  selector: "app-archives-print",
  templateUrl: "./archives-print.component.html",
  encapsulation: ViewEncapsulation.None,
  styles: [`
    .archive-print-page {
      direction: rtl;
      background: #fff;
      min-height: 100vh;
      color: #111;
      font-family: Tahoma, Arial, sans-serif;
    }

    .archive-print-toolbar {
      display: flex;
      align-items: center;
      justify-content: space-between;
      gap: 12px;
      margin: 20px auto;
      max-width: 900px;
      padding: 14px;
      border: 1px solid #ddd;
      border-radius: 8px;
      background: #f8f9fa;
    }

    .archive-print-paper {
      width: 100%;
      max-width: 900px;
      margin: 0 auto 30px;
      background: #fff;
      box-sizing: border-box;
      line-height: 2;
      font-size: 13px;
    }

    .archive-print-header {
      text-align: center;
      border-bottom: 2px solid #222;
      padding-bottom: 10px;
      margin-bottom: 18px;
    }

    .archive-print-title {
      font-size: 20px;
      font-weight: 700;
      margin: 0;
    }

    .archive-print-code {
      font-size: 12px;
      margin-top: 4px;
    }

    .archive-print-meta {
      display: grid;
      grid-template-columns: 1fr 1fr;
      gap: 7px 20px;
      margin-bottom: 18px;
    }

    .archive-print-section {
      margin-top: 18px;
      page-break-inside: auto;
    }

    .archive-print-section-title {
      font-weight: 700;
      border-bottom: 1px solid #777;
      padding-bottom: 4px;
      margin-bottom: 8px;
    }

    .archive-print-text {
      white-space: pre-wrap;
      overflow-wrap: anywhere;
      word-break: normal;
      text-align: justify;
    }

    .archive-print-answer {
      border: 1px solid #999;
      padding: 10px 12px;
      margin-top: 12px;
      border-radius: 4px;
      break-inside: avoid;
      page-break-inside: avoid;
    }

    .archive-print-answer-meta {
      font-size: 11px;
      color: #444;
      margin-bottom: 7px;
    }

    .archive-print-stamp {
      text-align: left;
      margin-top: 30px;
      break-inside: avoid;
      page-break-inside: avoid;
    }

    .archive-print-stamp img {
      width: 110px;
      height: auto;
    }

    .archive-print-loading {
      text-align: center;
      padding: 50px;
    }

    @media print {
      body {
        background: #fff !important;
        margin: 0 !important;
      }

      html, body {
        padding: 0 !important;
      }

      app-nav-menu,
      .archive-print-toolbar,
      .archive-print-back {
        display: none !important;
      }

      main.container-fluid {
        padding: 0 !important;
        margin: 0 !important;
      }

      .archive-print-paper {
        max-width: none;
        margin: 0;
      }

      .archive-print-page {
        min-height: auto;
      }
    }

  `]
})
export class ArchivesPrintComponent implements OnInit {
  question?: GetQuestionPrintDto;
  format: "A5" | "A4" = "A5";
  loading = true;
  error = "";

  get questionCode(): string {
    return this.question?.id != null
      ? this.question.id.toString().padStart(8, "0")
      : "00000000";
  }

  constructor(
    private route: ActivatedRoute,
    private printService: GetQuestionPrintService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get("id"));

    if (!id) {
      this.error = "شناسه استفتاء نامعتبر است.";
      this.loading = false;
      return;
    }

    this.printService.GetPrint(id).subscribe({
      next: (res) => {
        if (res?.isSuccess && res.data) {
          this.question = res.data;
        } else {
          this.error = res?.message || "اطلاعات استفتاء دریافت نشد.";
        }
        this.loading = false;
      },
      error: (err) => {
        this.error = err?.error?.message || "خطا در دریافت اطلاعات استفتاء.";
        this.loading = false;
      }
    });
  }

  setFormat(format: "A5" | "A4"): void {
    this.format = format;
  }

  print(): void {
    const paper = document.querySelector(".archive-print-paper") as HTMLElement | null;

    if (!paper) {
      return;
    }

    // Print only the document itself in a clean window.
    // This prevents the Angular application shell from creating an empty first page.
    const printWindow = window.open("", "_blank", "width=900,height=900");

    if (!printWindow) {
      alert("پنجره چاپ توسط مرورگر مسدود شده است. لطفاً اجازه باز شدن پنجره را بدهید.");
      return;
    }

    const pageSize = this.format === "A4" ? "A4" : "A5";
    const pageMargin = this.format === "A4" ? "14mm" : "12mm";

    const printHtml =
      '<!doctype html>' +
      '<html lang="fa" dir="rtl">' +
      '<head>' +
      '<meta charset="utf-8">' +
      '<title>استفتاء</title>' +
      '<style>' +
      '@page { size: ' + pageSize + ' portrait; margin: ' + pageMargin + '; }' +
      '* { box-sizing: border-box; }' +
      'html, body { margin: 0; padding: 0; background: #fff; }' +
      'body { direction: rtl; color: #111; font-family: Tahoma, Arial, sans-serif; font-size: 13px; line-height: 2; }' +
      '.archive-print-paper { width: 100%; margin: 0; background: #fff; }' +
      '.archive-print-header { text-align: center; border-bottom: 2px solid #222; padding-bottom: 10px; margin-bottom: 18px; }' +
      '.archive-print-title { font-size: 20px; font-weight: 700; margin: 0; }' +
      '.archive-print-code { font-size: 12px; margin-top: 4px; }' +
      '.archive-print-meta { display: grid; grid-template-columns: 1fr 1fr; gap: 7px 20px; margin-bottom: 18px; }' +
      '.archive-print-section { margin-top: 18px; }' +
      '.archive-print-section-title { font-weight: 700; border-bottom: 1px solid #777; padding-bottom: 4px; margin-bottom: 8px; }' +
      '.archive-print-text { white-space: pre-wrap; overflow-wrap: anywhere; word-break: normal; text-align: justify; }' +
      '.archive-print-answer { border: 1px solid #999; padding: 10px 12px; margin-top: 12px; border-radius: 4px; break-inside: avoid; page-break-inside: avoid; }' +
      '.archive-print-answer-meta { font-size: 11px; color: #444; margin-bottom: 7px; }' +
      '.archive-print-stamp { text-align: left; margin-top: 30px; break-inside: avoid; page-break-inside: avoid; }' +
      '.archive-print-stamp img { width: 110px; height: auto; }' +
      '</style>' +
      '</head>' +
      '<body>' +
      paper.outerHTML +
      '</body>' +
      '</html>';

    printWindow.document.open();
    printWindow.document.write(printHtml);
    printWindow.document.close();

    const startPrint = () => {
      setTimeout(() => {
        printWindow.focus();
        printWindow.print();
        printWindow.close();
      }, 150);
    };

    const images = Array.from(printWindow.document.images);

    if (images.length === 0 || images.every(image => image.complete)) {
      startPrint();
    } else {
      let remaining = images.length;

      images.forEach(image => {
        const done = () => {
          remaining--;
          if (remaining === 0) {
            startPrint();
          }
        };

        image.addEventListener("load", done, { once: true });
        image.addEventListener("error", done, { once: true });
      });
    }
  }

  goBack(): void {
    window.history.back();
  }
}
