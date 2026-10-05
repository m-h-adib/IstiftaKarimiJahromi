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
    const questionId = Number(this.route.snapshot.paramMap.get("id"));

    if (!questionId) {
      return;
    }

    this.printService.GetPrintPdf(questionId, this.format).subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const printWindow = window.open(url, "_blank");

        if (!printWindow) {
          URL.revokeObjectURL(url);
          alert("پنجره PDF توسط مرورگر مسدود شده است. لطفاً اجازه باز شدن پنجره را بدهید.");
          return;
        }

        setTimeout(() => URL.revokeObjectURL(url), 60000);
      },
      error: () => {
        alert("تولید فایل PDF چاپ با خطا مواجه شد.");
      }
    });
  }

  goBack(): void {
    window.history.back();
  }
}
