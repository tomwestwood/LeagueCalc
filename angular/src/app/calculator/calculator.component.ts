import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { LeagueTable } from '../models/leagueTable';
import { LeagueTableEntry } from '../models/leagueTableEntry';
import { LeagueFileConverterService } from '../services/league-file-converter.service';
import { TeamResultsComponent } from '../team-results/team-results.component';

@Component({
  selector: 'app-calculator',
  templateUrl: './calculator.component.html',
  styleUrls: ['./calculator.component.scss']
})
export class CalculatorComponent {
  file: File | undefined;
  leagueTable: MatTableDataSource<LeagueTableEntry> | undefined;
  loading: boolean = false;
  displayedColumns: string[] = ['teamPosition', 'teamName', 'goalsScored', 'goalsConceded', 'goalDifference', 'points', 'options'];

  constructor(private leagueFileConverterService: LeagueFileConverterService, public dialog: MatDialog, private snackBar: MatSnackBar) { }

  uploadFile(event: any) {
    this.file = event.target.files[0];
  }

  calculateLeagueFromFile() {
    this.loading = true;
    this.leagueFileConverterService.convertFile(this.file).subscribe(
      (leagueTable: LeagueTable) => {
        setTimeout(() => {
          this.leagueTable = new MatTableDataSource(leagueTable.leagueTableEntries);
          this.loading = false;
        }, 2000);
      },
      error => {
        this.snackBar.open(`Error uploading file... ${this.getFriendlyError(error.error)}`, 'Dismiss', { duration: 3000 });
        this.loading = false;
      }
    );
  }

  showResults(team: LeagueTableEntry) {
    this.dialog.open(TeamResultsComponent, {
      data: team
    });
  }

  private getFriendlyError(error: string) {
    if (error.length > 100)
      error = `${error.substring(0, 200)}...`;

    return error;
  }
}
