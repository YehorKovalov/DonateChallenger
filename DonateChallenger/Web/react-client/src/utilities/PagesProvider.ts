export function formPages(pagesNumber: number): number[] {
     const result = [];
     for (let i = 1; i <= pagesNumber; i++)
     {
          result.push(i);
     }

     return result;
}