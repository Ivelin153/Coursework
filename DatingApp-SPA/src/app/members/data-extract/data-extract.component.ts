import { Component, OnInit, Input } from '@angular/core';
import { FileModel } from 'src/app/_models/File';
import { UserService } from 'src/app/_services/user.service';
import { AuthService } from 'src/app/_services/auth.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-data-extract',
  templateUrl: './data-extract.component.html',
  styleUrls: ['./data-extract.component.css']
})
export class DataExtractComponent implements OnInit {
  @Input() files: FileModel[];
  data: any;
  allLines: string[];
  associationRules: string[][];
  itemSet: string[][];
  counter: number;
  perc: any;

  constructor(private userService: UserService, private authService: AuthService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.counter = 0;
    this.perc = parseFloat('100');
  }

  extractData(event) {
    const target = event.target || event.srcElement || event.currentTarget;
    const idAttr = target.attributes.id;
    const value = idAttr.nodeValue;
    console.log(this.authService.currentUser.id);

    const responseData = this.userService.extractData(this.authService.currentUser.id, value).subscribe(res => {
      this.data = res;
    });

    this.associationRules = this.data?.associationRules;
    this.allLines = this.data?.allLines;
    this.itemSet = this.data?.itemSet;
  }

  clearData() {
    this.allLines = null;
    this.itemSet = null;
    this.associationRules = null;
  }
}
