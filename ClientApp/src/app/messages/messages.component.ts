import { Component, OnInit } from '@angular/core';
import {
  MessagesService,
  MessageLevel,
  Message,
} from '../_services/messages.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css'],
})
export class MessagesComponent implements OnInit {
  constructor(public msgService: MessagesService) {}

  ngOnInit(): void {}
  msgIsLevelOf(message: Message, messageLevel: string): boolean {
    let enumLevel: MessageLevel;
    switch (messageLevel) {
      case 'Error':
        enumLevel = MessageLevel.Error;
        break;
      case 'Info':
        enumLevel = MessageLevel.Info;
        break;
      case 'Success':
        enumLevel = MessageLevel.Success;
        break;
      default:
        enumLevel = MessageLevel.Info;
        break;
    }
    return message.level == enumLevel;
  }
  getIcon(message: Message): string {
    let icon = 'info';
    switch (message.level) {
      case MessageLevel.Error:
        icon = 'error';
        break;
      case MessageLevel.Info:
        icon = 'info';
        break;
      case MessageLevel.Success:
        icon = 'check_circle';
        break;
    }
    return icon;
  }
}
