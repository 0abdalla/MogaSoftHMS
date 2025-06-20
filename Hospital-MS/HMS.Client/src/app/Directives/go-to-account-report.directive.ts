import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appGoToAccountReport]'
})
export class GoToAccountReportDirective {
  @Input() accountId: number;
  @Input() isGroup: boolean = true;
  constructor(private elem: ElementRef, private renderer: Renderer2) { }
  ngOnInit(): void {
    this.goToAccountReport()
  }
  goToAccountReport() {
    if (this.accountId) {
      this.renderer.setStyle(this.elem.nativeElement, 'text-decoration', 'underline');
      this.renderer.addClass(this.elem.nativeElement, 'cursor-pointer');
      this.renderer.listen(this.elem.nativeElement, 'click', () => {
        const url = `/general-accounts/accounts-general-ledger?AccountId=${this.accountId}`;
        window.open(url, '_blank');
      });
    }
  }
}
