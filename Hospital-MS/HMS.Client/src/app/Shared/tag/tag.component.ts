import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tag',
  templateUrl: './tag.component.html',
  styleUrl: './tag.component.css'
})
export class TagComponent {
  @Input() tags: string;
  @Input() header: string;
  @Input() showWithMore: boolean;

  Taglist: any[] = [];
  showMorelist: any[] = [];
  list: any[] = [];

  constructor() { }

  ngOnInit(): void {
    if (!this.tags || this.tags.length === 0) return;

    this.Taglist = this.tags.split(';;;');

    if (this.showWithMore && this.Taglist.length > 2) {
      this.list = this.Taglist.slice(0, 2);
      this.showMorelist = this.Taglist.slice(2);
    } else {
      this.list = this.Taglist;
    }
  }
}
