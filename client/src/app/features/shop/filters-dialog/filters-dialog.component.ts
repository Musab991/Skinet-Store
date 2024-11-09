import { Component, inject } from '@angular/core';
import { ShopService } from '../../../core/services/shop.service';
import { MatDivider } from '@angular/material/divider';
import { MatSelectionList } from '@angular/material/list';
import { MatListOption } from '@angular/material/list';
import { MatButton } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormsModule } from '@angular/forms';
@Component({
  selector: 'app-filters-dialog',
  standalone: true,
  imports: [
    MatDivider,
    MatListOption,
    MatSelectionList,
    MatButton,
    FormsModule,
    MatListOption,
  ],
  templateUrl: './filters-dialog.component.html',
  styleUrl: './filters-dialog.component.scss',
})
export class FiltersDialogComponent {
  shopService = inject(ShopService);

  private dialogRef = inject(MatDialogRef<FiltersDialogComponent>);
  data = inject(MAT_DIALOG_DATA);

  selectedBrands: string[] = [...this.data.selectedBrands]; // Create a shallow copy
  selectedTypes: string[] = [...this.data.selectedTypes]; // Create a shallow copy

  applyFilters() {
    console.log('Selected Brands:', this.selectedBrands);
    console.log('Selected Types:', this.selectedTypes);
    this.dialogRef.close({
      selectedBrands: this.selectedBrands,
      selectedTypes: this.selectedTypes,
    });
  }
}
