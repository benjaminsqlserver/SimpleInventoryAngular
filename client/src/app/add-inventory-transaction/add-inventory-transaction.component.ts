import { Component, Injector } from '@angular/core';
import { AddInventoryTransactionGenerated } from './add-inventory-transaction-generated.component';

@Component({
  selector: 'page-add-inventory-transaction',
  templateUrl: './add-inventory-transaction.component.html'
})
export class AddInventoryTransactionComponent extends AddInventoryTransactionGenerated {
  constructor(injector: Injector) {
    super(injector);
  }
}
