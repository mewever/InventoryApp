import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { InventoryItemsService, InventoryItem } from '../services/inventoryItemsService.service';

@Component({
  selector: 'app-item-receive',
  standalone: false,
  templateUrl: './item-receive.html',
  styleUrl: './item-receive.css',
})
export class ItemReceive {
  private route = inject(ActivatedRoute);
  inventoryItemId: number = 0;
  item: InventoryItem | null = null;
  errorMessage: string | null = null;
  referenceNumber: string = '';
  description: string = '';
  quantityReceived: number = 0;
  amountPaid: number = 0;

  constructor(private inventoryItemsService: InventoryItemsService, private router: Router) {
    // Get the id from the route parameter
    var idParm = this.route.snapshot.paramMap.get('id');
    if (idParm != null) {
      this.inventoryItemId = +idParm;
    }
  }

  ngOnInit(): void {
    this.inventoryItemsService.details(this.inventoryItemId).subscribe(response => {
      if (!response.isSuccessful) {
        this.errorMessage = "Could not load the item.";
        return;
      }
      if (response.response != null) {
        this.item = response.response;
      }
    });
  }

  save(): void {
    if (this.item == null) {
      return;
    }
    this.errorMessage = null;
    this.inventoryItemsService.receive(this.inventoryItemId, this.referenceNumber, this.description, this.quantityReceived, this.amountPaid).subscribe(response => {
      if (!response.isSuccessful) {
        this.errorMessage = response.errorMessage;
        return;
      }
      this.router.navigate(['items']);
    });
  }
}
