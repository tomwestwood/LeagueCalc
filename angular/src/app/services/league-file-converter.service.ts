import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class LeagueFileConverterService {
  protected calculationUrl: string = '/api/league-calculator';
  constructor(private http: HttpClient) { }

  convertFile(file: any): Observable<any> {
    const formData = new FormData();
    formData.append("file", file, file.name);
    return this.http.post(this.calculationUrl, formData)
  }
}
