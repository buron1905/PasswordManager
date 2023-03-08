import { AfterViewInit, Component, ElementRef, EventEmitter, OnDestroy, OnInit, Output, ViewChild } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ClipboardService } from 'ngx-clipboard';
import { ToastrService } from 'ngx-toastr';
import { toArray } from 'rxjs';
import { PasswordGeneratorService } from '../services/password-generator.service';

@Component({
  selector: 'app-password-generator',
  templateUrl: './password-generator.component.html',
  styleUrls: ['./password-generator.component.css']
})
export class PasswordGeneratorComponent implements OnInit, OnDestroy, AfterViewInit {
  @Output() newPasswordEvent = new EventEmitter<string>();

  generatorForm: FormGroup;
  submitted = false;
  loading = false;

  generatedPassword: string = "";

  private resizeObserver: ResizeObserver;

  constructor(private fb: FormBuilder, private toastrService: ToastrService, private passwordGeneratorService: PasswordGeneratorService, private clipboardService: ClipboardService) {
    this.generatorForm = this.fb.group({
      passwordLength: [12, [Validators.required, Validators.min(5), Validators.max(256)]],
      useNumbers: [true, [Validators.nullValidator]],
      useSpecialChars: [true, [Validators.nullValidator]],
      useUppercase: [true, [Validators.nullValidator]],
      useLowercase: [true, [Validators.nullValidator]],
    });

    this.resizeObserver = new ResizeObserver(this.scaleFont);
  }

  ngOnInit(): void {
    this.onChanges();
    this.generate();
  }

  ngAfterViewInit(): void {
    this.resizeObserver.observe(document.getElementById("generatedText"));
  }

  ngOnDestroy(): void {
    this.resizeObserver.unobserve(document.getElementById("generatedTextContainer"));
  }

  scaleFont(): void {
    let generatedTextElement = document.getElementById("generatedText");
    let originalFontSize = parseFloat(window.getComputedStyle(generatedTextElement).getPropertyValue("font-size")).toFixed(0);
    let textHeight = generatedTextElement.offsetHeight;
    let containerHeight = document.getElementById("generatedTextContainer").offsetHeight;
    let newFontSize = parseFloat(originalFontSize);
    
    //while (textHeight > (containerHeight / 100 * 90) || textHeight < (containerHeight / 100 * 60)) {
      if (textHeight > (containerHeight / 100 * 98)) {
        newFontSize -= 1;
        generatedTextElement.style.fontSize = `${newFontSize}px`;
      }
      else if (textHeight < (containerHeight / 100 * 50)) {
        newFontSize += 1;
        generatedTextElement.style.fontSize = `${newFontSize}px`;
      }
      //textHeight = generatedTextElement.offsetHeight;
    //}
  }

  generate(): void {
    this.submitted = true;
    if (this.generatorForm.invalid) {
      return;
    }
    //this.loading = true;

    this.generatedPassword = this.passwordGeneratorService.generatePasswordFromModel(this.generatorForm.value);
    this.newPasswordEvent.emit(this.generatedPassword);

    //this.passwordGeneratorService.generatePassword(this.generatorForm.value).subscribe(
    //  data => {
    //    this.generatedPassword = data.password;
    //    this.loading = false;
    //  },
    //  error => {
    //    this.toastrService.error(`Error when generating new password occured.\nError:${error}`);
    //    this.loading = false;
    //  }
    //);
  }

  get f(): { [key: string]: AbstractControl } {
    return this.generatorForm.controls;
  }

  get passwordLength() {
    return this.generatorForm.get('passwordLength');
  }

  get useNumbers() {
    return this.generatorForm.get('useNumbers');
  }

  get useSpecialChars() {
    return this.generatorForm.get('useSpecialChars');
  }

  get useUppercase() {
    return this.generatorForm.get('useUppercase');
  }

  get useLowercase() {
    return this.generatorForm.get('useLowercase');
  }

  copyToClipboard(text: string): void {
    this.clipboardService.copyFromContent(text);
    this.toastrService.success('Copied to clipboard');
  }

  onChanges(): void {
    this.generatorForm.valueChanges.subscribe(() => {
      this.generate();
    });
  }

}
