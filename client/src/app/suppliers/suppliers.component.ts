import { Component, Injector } from '@angular/core';
import { SuppliersGenerated } from './suppliers-generated.component';

@Component({
  selector: 'page-suppliers',
  templateUrl: './suppliers.component.html'
})
export class SuppliersComponent extends SuppliersGenerated {
  constructor(injector: Injector) {
    super(injector);
  }
}
