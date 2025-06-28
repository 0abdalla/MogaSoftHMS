import { Directive, ElementRef, Input, Renderer2 } from '@angular/core';

@Directive({
  selector: '[appWorkflowStatus]'
})
export class WorkflowStatusDirective {
  @Input() nameEN: string = '';
  @Input() nameAR: string = '';
  @Input() id?: number | null = null;

  constructor(private elem: ElementRef, private renderer: Renderer2) { }

  ngOnInit(): void {
    this.renderStatus();
  }

  private renderStatus() {
    const text = this.nameAR || this.nameEN || 'غير معروف';
    const statusColors: Record<number, { bg: string; fg: string }> = {
      //Finance
      7: { bg: '#FFF3CD', fg: '#856404' }, // Pending
      8: { bg: '#F8D7DA', fg: '#721C24' }, // Cancelled
      9: { bg: '#D1ECF1', fg: '#0C5460' }, // Waiting Payment
      10: { bg: '#D4EDDA', fg: '#155724' }, // Paid
      //HR
      11: { bg: '#FFF3CD', fg: '#856404' }, // Pending
      12: { bg: '#F8D7DA', fg: '#721C24' }, // Rejected
      13: { bg: '#D1ECF1', fg: '#0C5460' }, // Approved
      14: { bg: '#D4EDDA', fg: '#155724' }, // Completed
      //payment
      15: { bg: '#28a745', fg: '#ffffff' }, // Paid
      16: { bg: '#dc3545', fg: '#ffffff' }, // UnPaid
      //job
      17: { bg: '#D1ECF1', fg: '#0C5460' }, // Active
      18: { bg: '#FFF3CD', fg: '#856404' }, // Vacation
      19: { bg: '#F8D7DA', fg: '#721C24' }, // Exit
      20: { bg: '#F8D7DA', fg: '#721C24' }, // Escape
      21: { bg: '#FFF3CD', fg: '#856404' }, // Pending
      22: { bg: '#F8D7DA', fg: '#721C24' }, // Case
      23: { bg: '#F8D7DA', fg: '#721C24' }, // DontReturnVacation

      24: { bg: '#FFF3CD', fg: '#856404' }, // Pending
      25: { bg: '#F8D7DA', fg: '#721C24' }, // Rejected
      26: { bg: '#D1ECF1', fg: '#0C5460' }, // Approved
      27: { bg: '#D4EDDA', fg: '#155724' }, // Completed
    };
    const defaultColors = { bg: '#E2E3E5', fg: '#383D41' };
    const colors = this.id && statusColors[this.id] ? statusColors[this.id] : defaultColors;
    const el = this.elem.nativeElement;
    this.renderer.setStyle(el, 'background-color', colors.bg);
    this.renderer.setStyle(el, 'color', colors.fg);
    this.renderer.setStyle(el, 'padding', '4px 8px');
    this.renderer.setStyle(el, 'border-radius', '4px');
    this.renderer.setStyle(el, 'display', 'inline-block');
    this.renderer.setStyle(el, 'font-weight', 'bold');
    this.renderer.setStyle(el, 'white-space', 'nowrap');
    const div = this.renderer.createElement('div');
    const textNode = this.renderer.createText(text);
    this.renderer.appendChild(div, textNode);
    this.renderer.appendChild(el, div);
  }
}
