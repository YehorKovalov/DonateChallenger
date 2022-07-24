import { injectable } from "inversify";

export interface LocalStorageService {
     get<T>(key: string): T | null;
     set<T>(key: string, value: T): void;
     remove(key: string): void;
}

@injectable()
export default class DefaultLocalStorageService implements LocalStorageService {

     get<T>(key: string): T | null {
          const value = localStorage.getItem(key);
          if (value === null) {
               return null;
          }

          return JSON.parse(value);
     }

     set<T>(key: string, value: T): void {
          localStorage.setItem(key, JSON.stringify(value));
     }

     remove(key: string): void {
          localStorage.removeItem(key)
     }
}