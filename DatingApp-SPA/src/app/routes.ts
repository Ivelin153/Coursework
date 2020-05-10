import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberEditResolver } from './_resolvers/member-edit.resolver';
import { PreventUnsavedChanges } from './_guards/prevent-unsaved-changes.guard';
import { FileUploadComponent } from './members/file-upload/file-upload.component';
import { DataExtractComponent } from './members/data-extract/data-extract.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {
                path: 'members/:id', component: MemberDetailComponent,
                resolve: { user: MemberDetailResolver }
            },
            {
                path: 'member/edit', component: MemberEditComponent,
                resolve: { user: MemberEditResolver }, canDeactivate: [PreventUnsavedChanges]
            },
            { path: 'fileUpload', component: FileUploadComponent},
            { path: 'apriori', component: DataExtractComponent}
        ]
    },
    { path: '**', redirectTo: '', pathMatch: 'full' }
];
