import { Component, OnInit, signal, computed } from '@angular/core';
import { Transaction } from '../../models/transaction';
import { CurrencyPipe, DatePipe, NgClass } from '@angular/common';
import { TransactionService } from '../../services/transaction.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-transaction-list',
  imports: [DatePipe, CurrencyPipe, NgClass],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionListComponent implements OnInit {
  transactions = signal<Transaction[]>([]);
  constructor(
    private transactionService: TransactionService,
    private router: Router,
  ) {}
  ngOnInit(): void {
    this.loadTransactions();
  }

  loadTransactions(): void {
    this.transactionService.getAll().subscribe((data) => {
      this.transactions.set(data);
    });
  }

  totalIncome = computed<number>(() =>
    this.transactions()
      .filter((t) => t.type === 'Income')
      .reduce((sum, t) => sum + t.amount, 0),
  );

  totalExpenses = computed<number>(() =>
    this.transactions()
      .filter((t) => t.type === 'Expense')
      .reduce((sum, t) => sum + t.amount, 0),
  );
  netBalance = computed<number>(() => this.totalIncome() - this.totalExpenses());

  editTransaction(transaction: Transaction) {
    if (transaction.id) {
      this.router.navigate(['/edit/' + transaction.id]);
    }
  }
  deleteTransaction(transaction: Transaction) {
    if (transaction.id) {
      if (confirm('Are you sure you want to delete this transaction?')) {
        this.transactionService.delete(transaction.id).subscribe({
          next: () => this.loadTransactions(),
          error: (error) => {
            console.log('Error is ' + error);
          },
        });
      }
    }
  }
}
