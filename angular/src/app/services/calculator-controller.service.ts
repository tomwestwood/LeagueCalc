import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BehaviorSubject } from 'rxjs';
import { LeagueTable } from '../models/leagueTable';
import { LeagueFileConverterService } from './league-file-converter.service';

@Injectable({ providedIn: 'root' })
export class CalculatorControllerService {
  loading: BehaviorSubject<boolean> = new BehaviorSubject<boolean>(false);
  leagueTable: BehaviorSubject<LeagueTable | undefined> = new BehaviorSubject<LeagueTable | undefined>(undefined);

  constructor(private leagueFileConverterService: LeagueFileConverterService, private snackBar: MatSnackBar) { }

  calculateLeagueFromFile(file: any): void {
    this.loading.next(true);

    this.leagueFileConverterService.convertFile(file).subscribe(
      (leagueTable: LeagueTable) => {
        setTimeout(() => {
          this.leagueTable.next(leagueTable);
          this.loading.next(false);
        }, 2000);
      },
      error => {
        this.snackBar.open(`Error uploading file... ${this.getFriendlyError(error.error)}`, 'Dismiss', { duration: 3000 });
        this.loading.next(false);
      }
    );
  }

  private getFriendlyError(error: string) {
    if (error.length > 100)
      error = `${error.substring(0, 200)}...`;

    return error;
  }
}
