export class Pagination<T> {
  pageIndex: number;
  pageSize: number;
  count: number;
  data: T[];

  // Constructor to initialize default values
  constructor(
    pageIndex: number = 1, // Default to page 1
    pageSize: number = 10, // Default to 10 items per page
    count: number = 0, // Default to 0 items
    data: T[] = [] // Default to empty array
  ) {
    this.pageIndex = pageIndex;
    this.pageSize = pageSize;
    this.count = count;
    this.data = data;
  }
}
