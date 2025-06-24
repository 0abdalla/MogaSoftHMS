import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appColorWithStatusDirective]'
})
export class ColorWithStatusDirectiveDirective {
  @Input() status: string = '';
  @Input() text: any = null;
  constructor(private elem: ElementRef, private renderer: Renderer2) { }

  ngOnInit(): void {
    this.checkStatus()
  }

  checkStatus() {
    if (this.status && this.status == 'IsLocked') {
      switch (this.text) {
        case true:
          this.renderer.setAttribute(this.elem.nativeElement, 'class', 'status-box green');
          var div = this.renderer.createElement('div');
          this.renderer.addClass(div, 'ms-2');
          var text = this.renderer.createText('مرحل');
          this.renderer.appendChild(div, text);
          this.renderer.appendChild(this.elem.nativeElement, div);
          break;

        case false:
          this.renderer.setAttribute(this.elem.nativeElement, 'class', 'status-box orange');
          div = this.renderer.createElement('div');
          this.renderer.addClass(div, 'ms-2');
          text = this.renderer.createText('غير مرحل');
          this.renderer.appendChild(div, text);
          this.renderer.appendChild(this.elem.nativeElement, div);
          break;
        default:
          break;
      }

    }
    else if (this.status && this.status.toLowerCase() == 'isapproved') {
      var label = '';
      var style = 'gray'
      if (this.text == true) {
        label = 'مقبول';
        style = 'green';
      }
      else if (this.text == false) {
        label = 'ملغي';
        style = 'red';
      }
      else {
        label = 'غير معروف';
        style = 'orange';
      }

      this.renderer.setAttribute(this.elem.nativeElement, 'class', 'status-box text-nowrap ' + style);
      var div = this.renderer.createElement('div');
      var text = this.renderer.createText(label);
      this.renderer.appendChild(div, text);
      this.renderer.appendChild(this.elem.nativeElement, div);

    }
    else if (this.status && this.status == 'receiptsStatus') {
      switch (this.text) {
        case '':
          this.renderer.setAttribute(this.elem.nativeElement, 'class', 'bg-success-gradient w-icon');
          break;
        case 'y':
          this.renderer.setAttribute(this.elem.nativeElement, 'class', 'bg-warning-gradient w-icon');

          break;
        default:
          this.renderer.setAttribute(this.elem.nativeElement, 'class', 'bg-secondary-gradient w-icon');

          break;
      }
      this.elem.nativeElement.innerHTML = '&nbsp;<i class="fas fa-chart-pie"></i>';
    }
    else if (this.status && this.status.toLowerCase() == 'iscancelled') {
      var label = '';
      if (this.text == true) {
        label = 'ملغي';
        style = 'red';
      }
      this.renderer.setAttribute(this.elem.nativeElement, 'class', 'status-box text-nowrap ' + style);
      var div = this.renderer.createElement('div');
      var text = this.renderer.createText(label);
      this.renderer.appendChild(div, text);
      this.renderer.appendChild(this.elem.nativeElement, div);
    }
  }
}
