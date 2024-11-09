import { Component, OnInit, inject } from '@angular/core';
import { ShopService } from '../../core/services/shop.service';
import { Product } from '../../models/product';
import { ProductItemComponent } from './product-item/product-item.component';
import { MatDialog } from '@angular/material/dialog';
import { FiltersDialogComponent } from './filters-dialog/filters-dialog.component';
import { MatButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import {
  MatListOption,
  MatSelectionList,
  MatSelectionListChange,
} from '@angular/material/list';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { ShopParams } from '../../models/shopParams';
import { Pagination } from '../../models/pagination';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-shop',
  standalone: true,
  imports: [
    ProductItemComponent,
    MatButton,
    MatIcon,
    MatMenu,
    MatSelectionList,
    MatListOption,
    MatMenuTrigger,
    MatPaginator,
    MatPaginatorModule,
    FormsModule,
    RouterLink,
  ],
  templateUrl: './shop.component.html',
  styleUrl: './shop.component.scss',
})
export class ShopComponent implements OnInit {
  private shopService = inject(ShopService);
  products: Pagination<Product> = new Pagination<Product>(); // Initialized using the constructor
  private dialogService = inject(MatDialog);
  sortOptions: any[] = [
    { name: 'Alphabetical', value: 'name' },
    { name: 'Price: low-high', value: 'priceAsc' },
    { name: 'Price: high-low', value: 'priceDesc' },
  ];
  readonly pageSizeOptions: number[] = [5, 10, 15, 20];
  shopParams: ShopParams = new ShopParams();

  pageChanged(event: PageEvent) {
    this.shopParams.pageIndex = event.pageIndex + 1;
    this.shopParams.pageSize = event.pageSize;
    this.getProducts();
  }
  onSortChange(event: MatSelectionListChange) {
    const selectedOption = event.options[0];
    if (selectedOption) {
      this.shopParams.sort = selectedOption.value;
      this.shopParams.resetPageNumberToOne();
      this.getProducts();
    }
  }
  openFiltersDialog() {
    const dialogRef = this.dialogService.open(FiltersDialogComponent, {
      minWidth: '500px',
      data: {
        selectedBrands: [...this.shopParams.brands],
        selectedTypes: [...this.shopParams.types],
      },
    });
    dialogRef.afterClosed().subscribe({
      next: (result) => {
        if (result) {
          this.shopParams.brands = result.selectedBrands;
          this.shopParams.types = result.selectedTypes;
          this.shopParams.resetPageNumberToOne();
          this.getProducts();
        }
      },
    });
  }

  onSearchChange() {
    this.shopParams.resetPageNumberToOne();
    this.getProducts();
  }
  filterResults(filter: string) {
    this.shopParams.search = filter;
    this.getProducts();
  }

  ngOnInit(): void {
    this.initializeShop();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response) => {
        this.products.data = response.data;
        this.products.count = response.count;
      },
      error: (err) => console.error('Error from getProducts()', err),
    });
  }
  initializeShop() {
    this.shopService.getBrands();
    this.shopService.getTypes();
    this.getProducts();
  }
}
