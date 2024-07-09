import { Component, Injector } from '@angular/core';
import { InventoryTransactionsGenerated } from './inventory-transactions-generated.component';

@Component({
  selector: 'page-inventory-transactions',
  templateUrl: './inventory-transactions.component.html'
})
export class InventoryTransactionsComponent extends InventoryTransactionsGenerated {
  constructor(injector: Injector) {
    super(injector);
  }
}
