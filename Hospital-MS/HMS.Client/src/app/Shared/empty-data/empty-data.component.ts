import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-empty-data',
  templateUrl: './empty-data.component.html',
  styleUrl: './empty-data.component.css'
})
export class EmptyDataComponent {
  @Input() showEmptyData: boolean = false;
  @Input() showLoader: boolean = false;
}
