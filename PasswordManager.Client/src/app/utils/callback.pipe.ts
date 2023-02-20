import { Pipe, PipeTransform } from '@angular/core';
import { PasswordModel } from '../models/password.model';

@Pipe({
  name: 'callback',
  pure: false
})
export class CallbackPipe implements PipeTransform {
  transform(items: PasswordModel[], searchText: string, callback: (item: PasswordModel, filterText: string) => boolean): PasswordModel[] {
    if (!items || !callback) {
      return items;
    }
    return items.filter(item => callback(item, searchText));
  }

}
