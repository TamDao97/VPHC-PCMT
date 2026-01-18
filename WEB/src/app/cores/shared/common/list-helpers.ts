export function updateIndex(list: any[], fieldName: string = 'index', startIndex: number = 1): void {
    list.forEach((item, index) => {
      item[fieldName] = index + startIndex;
    });
  }

  export function sortList<T>(
    list: T[], 
    fieldName: keyof T, 
    order: 'asc' | 'desc' = 'asc'
  ): void {
    list.sort((a, b) => {
      const valueA = a[fieldName];
      const valueB = b[fieldName];
  
      if (typeof valueA === 'number' && typeof valueB === 'number') {
        return order === 'asc' ? valueA - valueB : valueB - valueA;
      } 
      if (typeof valueA === 'string' && typeof valueB === 'string') {
        return order === 'asc' 
          ? valueA.localeCompare(valueB) 
          : valueB.localeCompare(valueA);
      }
      return 0; // Không sắp xếp nếu không phải số hoặc chuỗi
    });
  }