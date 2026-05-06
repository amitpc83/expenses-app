import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction-form',
  imports: [ReactiveFormsModule],
  templateUrl: './transaction-form.html',
  styleUrl: './transaction-form.css',
})
export class TransactionFormComponent implements OnInit {
  transactionForm: FormGroup;
  editMode = false;
  transactionId?: number;

  expenseCategories: string[] = ['Food', 'Transportation', 'Entertainment'];
  incomeCategories: string[] = ['Salary', 'Freelance', 'Investment'];

  availableCategories: string[] = [];

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private transactionService: TransactionService,
    private activatedRoute: ActivatedRoute,
  ) {
    this.transactionForm = this.fb.group({
      type: ['Expense', Validators.required],
      category: ['', Validators.required],
      amount: ['', [Validators.required, Validators.min(0)]],
    });
  }

  ngOnInit(): void {
    const id = this.activatedRoute.snapshot.paramMap.get('id');

    if (id) {
      this.editMode = true;
      this.transactionId = +id;
      this.loadTransaction(this.transactionId);
    } else {
      this.updateAvailableCategories();
    }
  }

  onTypeChange() {
    this.updateAvailableCategories();
  }

  updateAvailableCategories() {
    const type = this.transactionForm.get('type')?.value;
    this.availableCategories = type === 'Expense' ? this.expenseCategories : this.incomeCategories;
    if (!this.editMode) {
      this.transactionForm.patchValue({ category: '' });
    }
  }
  loadTransaction(id: number) {
    this.transactionService.getById(id).subscribe({
      next: (data) => {
        this.transactionForm.patchValue({
          type: data.type,
          category: data.category,
          amount: data.amount,
        });
        this.updateAvailableCategories();
      },
      error: (error) => {
        console.log('Error - ' + error);
      },
    });
  }

  onSubmit() {
    if (this.transactionForm.valid) {
      var transaction = this.transactionForm.value;
      console.log(transaction);

      if (this.editMode && this.transactionId) {
        this.transactionService.edit(this.transactionId, transaction).subscribe({
          next: () => {
            this.routeToListingPage();
          },
          error: (err) => {
            console.log('Error :' + err);
          },
        });
      } else {
        this.transactionService.create(transaction).subscribe({
          next: () => {
            this.routeToListingPage();
          },
          error: (err) => {
            console.log('Error :' + err);
          },
        });
      }
    }
  }
  cancel() {
    this.routeToListingPage();
  }

  routeToListingPage() {
    this.router.navigate(['/transactions']);
  }
}
