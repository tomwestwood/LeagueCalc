import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LeagueTable } from '../models/leagueTable';

@Injectable({ providedIn: 'root' })
export class LeagueFileConverterService {
  protected calculationUrl: string = '/api/league-calculator';
  constructor(private http: HttpClient) { }

  convertFile(file: any): Observable<LeagueTable> {
    const formData = new FormData();
    formData.append("file", file, file.name);
    return this.http.post<LeagueTable>(this.calculationUrl, formData)
  }
}
