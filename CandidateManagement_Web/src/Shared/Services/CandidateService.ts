import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { catchError, map, Observable, throwError } from "rxjs";
import { Candidate } from "../Models/Candidate";
import { constants } from "../constants";
 

@Injectable({ providedIn: 'root' })
export class CandidateService {
  private base = constants.apiUrl + '/candidates';
  private http = inject(HttpClient);
 
  getCandidates() : Observable<Candidate[]>{
   return this.http.get<Candidate[]>(this.base).pipe(
       catchError( errorMessage => {
          console.log(errorMessage)
           return throwError(() => new Error(errorMessage));
       })
    )
   }

  upload(file: File){
    const form = new FormData();
    form.append('file', file);
    return this.http.post<Candidate[]>(`${this.base}/upload`, form);
  }


  add(candidate: Candidate){
    return this.http.post<Candidate>(`${this.base}/add`, candidate);
  }

  edit(candidate: Candidate){
    return this.http.put<Candidate>(`${this.base}/edit`, candidate);
  }

  export() {
    return this.http.get(`${this.base}/export`, { responseType: 'blob' });
  }
}