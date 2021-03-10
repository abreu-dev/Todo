import { Component, ContentChild, ElementRef, EventEmitter, OnInit, Output, TemplateRef } from '@angular/core';

import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';

import { EditModeDirective } from '@shared/directives/edit-mode.directive';
import { ViewModeDirective } from '@shared/directives/view-mode.directive';

import { Subject, fromEvent } from 'rxjs';
import { filter, switchMapTo, take } from 'rxjs/operators';

@UntilDestroy()
@Component({
    selector: 'app-editable',
    templateUrl: './editable.component.html',
    styleUrls: ['./editable.component.scss'],
})
export class EditableComponent implements OnInit {
    @Output() update = new EventEmitter();
    @ContentChild(ViewModeDirective) viewModeTpl!: ViewModeDirective;
    @ContentChild(EditModeDirective) editModeTpl!: EditModeDirective;

    mode: 'view' | 'edit' = 'view';

    editMode = new Subject();
    editMode$ = this.editMode.asObservable();

    constructor(private host: ElementRef) {}

    ngOnInit(): void {
        this.viewModeHandler();
        this.editModeHandler();
    }

    public get currentView(): TemplateRef<any> {
        return this.mode === 'view' ? this.viewModeTpl.tpl : this.editModeTpl.tpl;
    }

    private get element() {
        return this.host.nativeElement;
    }

    private viewModeHandler() {
        fromEvent(this.element, 'dblclick')
            .pipe(untilDestroyed(this))
            .subscribe(() => {
                this.mode = 'edit';
                this.editMode.next(true);
            });
    }

    private editModeHandler() {
        const clickOutside$ = fromEvent(document, 'click').pipe(
            filter(({ target }) => this.element.contains(target) === false),
            take(1)
        );

        this.editMode$.pipe(switchMapTo(clickOutside$), untilDestroyed(this)).subscribe(() => {
            this.update.next();
            this.mode = 'view';
        });
    }

    public toViewMode(): void {
        this.update.next();
        this.mode = 'view';
    }
}
