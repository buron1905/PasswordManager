import { Injectable } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { RouterStateSnapshot, TitleStrategy } from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class AppTitleSuffixService extends TitleStrategy {

  constructor(private readonly title: Title) {
    super();
  }

  override updateTitle(routerState: RouterStateSnapshot) {
    const title = this.buildTitle(routerState);
    if(title !== undefined)
      this.title.setTitle(`${title} - Password Manager`);
    else
      this.title.setTitle('Password Manager');
  }
  
}
