import { Pipe, PipeTransform } from '@angular/core';
import { PasswordModel } from '../models/password.model';

@Pipe({
  name: 'callback',
  pure: false
})
export class CallbackPipe implements PipeTransform {
  transform(items: PasswordModel[], searchText: string, searchFavorites: boolean,
    callback: (item: PasswordModel, filterText: string, searchFavorites: boolean) => boolean): PasswordModel[] {
    if (!items || !callback) {
      return items;
    }
    return items.filter(item => callback(item, searchText, searchFavorites));
  }

}
