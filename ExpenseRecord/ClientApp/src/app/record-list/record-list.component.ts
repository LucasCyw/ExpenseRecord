import { Component, OnDestroy, OnInit } from '@angular/core';
import { ExpenseRecord } from '../models/ExpenseRecord';
import { ExpenseRecordService } from '../services/expense-record.service';

@Component({
  selector: 'app-record-list',
  templateUrl: './record-list.component.html',
  styleUrls: ['./record-list.component.css']
})
export class ExpenseRecordListComponent implements OnInit, OnDestroy {
  public records: ExpenseRecord[] = [];
  public newRecord: ExpenseRecord;

  constructor(private expenseRecordService: ExpenseRecordService) {
    this.newRecord = {} as ExpenseRecord;
  }

  ngOnInit(): void {
    this.load();
  }

  ngOnDestroy() {

  }

  reload(): void {
    this.load();
  }

  private load(): void {
    this.expenseRecordService.getAll().subscribe({
      next: item => {
        this.records = item;
      }
    });
  }

  delete(id: string): void {
    const status = confirm(`Delete this item?`);
    if (status) {
      this.expenseRecordService.deleteOne(id).subscribe(() => {
        this.reload();
      });
    }
  }
  create(): void {
    this.expenseRecordService.createOne(this.newRecord).subscribe(() => {
      console.log("newrecord message");
      console.log(this.newRecord.description);
      console.log(this.newRecord.type);
      this.newRecord = {} as ExpenseRecord;
      this.reload();
    })
  }
}
