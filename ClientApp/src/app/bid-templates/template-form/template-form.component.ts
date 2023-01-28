import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BehaviorSubject, Observable } from 'rxjs';
import { BidTemplate } from 'src/app/_models/bid-template';

@Component({
  selector: 'app-template-form',
  templateUrl: './template-form.component.html',
  styleUrls: ['./template-form.component.css'],
})
export class TemplateFormComponent implements OnInit {
  @Input() templateInput: BidTemplate = new BidTemplate();
  @Output() formResult = new EventEmitter<BidTemplate>();
  templateForm: FormGroup = this.fb.group({
    description: [''],
  });
  /**
   *
   */
  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {}
  ngOnChanges() {
    this.templateForm.controls.description.setValue(
      this.templateInput.description
    );
  }
  onSubmit() {
    this.templateInput.description =
      this.templateForm.controls.description.value;
    this.formResult.emit(this.templateInput);
  }
}
