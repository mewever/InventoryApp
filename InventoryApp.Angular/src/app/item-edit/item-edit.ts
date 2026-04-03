import { Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { InventoryItemsService, InventoryItem } from '../services/inventoryItemsService.service';

@Component({
  selector: 'app-item-edit',
  standalone: false,
  templateUrl: './item-edit.html',
  styleUrl: './item-edit.css',
})
export class ItemEdit {
  private route = inject(ActivatedRoute);
  inventoryItemId: number = 0;
  item: InventoryItem | null = null;
  errorMessage: string | null = null;

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
    this.inventoryItemsService.update(this.item).subscribe(response => {
      if (!response.isSuccessful) {
        this.errorMessage = response.errorMessage;
        return;
      }
      this.router.navigate(['items']);
    });
  }
}
