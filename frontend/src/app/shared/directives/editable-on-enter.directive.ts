import { Directive, HostListener } from '@angular/core';

import { EditableComponent } from '@shared/components/editable/editable.component';

@Directive({
    selector: '[appEditableOnEnter]',
})
export class EditableOnEnterDirective {
    constructor(private editable: EditableComponent) {}

    @HostListener('keyup.enter')
    onEnter() {
        this.editable.toViewMode();
    }
}
