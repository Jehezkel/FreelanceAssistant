import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { BidTemplate } from 'src/app/_models/bid-template';

@Component({
  selector: 'app-template-form',
  templateUrl: './template-form.component.html',
  styleUrls: ['./template-form.component.css'],
})
export class TemplateFormComponent implements OnInit {
  @Input() template: BidTemplate = new BidTemplate();
  @Output() result = new EventEmitter<BidTemplate>();
  templateForm: FormGroup = this.fb.group({
    description: [''],
  });
  /**
   *
   */
  constructor(private fb: FormBuilder) {}
  ngOnInit(): void {
    console.log('started value', this.template);
    this.templateForm.controls.description.setValue(this.template.description);
  }
  onSubmit() {
    this.template.description = this.templateForm.controls.description.value;
    this.result.emit(this.template);
  }
}
