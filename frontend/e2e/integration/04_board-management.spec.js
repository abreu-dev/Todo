/// <reference types="Cypress" />

Cypress.Commands.add('login', function () {
    cy.request({
        method: 'POST',
        url: 'https://localhost:5001/api/accounts/login',
        body: {
            username: 'tester',
            password: 'Tester%123',
        },
    }).then(function (response) {
        localStorage.setItem('todo.user', JSON.stringify(response.body));
    });
});

describe('Board Management Page', function () {
    beforeEach(function () {
        cy.login();
        cy.visit('/home');
        cy.get('#board-0').click();
        cy.wait(2000);
    });
    it('should visit board page', function () {
        cy.url().should('include', '/board');
    });
    it('should display board title', function () {
        cy.get('#board-title').should('contain.text', 'Serise');
    });
    it('should update board title', function () {
        cy.get('#board-title').dblclick();
        cy.wait(2000);

        cy.get('#update-board-title').clear().type('Series {enter}');
        cy.wait(2000);

        cy.get('#board-title').should('contain.text', 'Series');
    });
    it('should add columns', function () {
        cy.get('#add-column').click();
        cy.wait(2000);

        cy.get('.todo-add-column-tile-menu-form > #column-title').type('To watch');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);

        cy.get('#column-0 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'To watch'
        );
        cy.wait(2000);

        cy.get('#add-column').click();
        cy.wait(2000);

        cy.get('.todo-add-column-tile-menu-form > #column-title').type('To watch');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);

        cy.get('#column-1 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'To watch'
        );
        cy.wait(2000);

        cy.get('#add-column').click();
        cy.wait(2000);

        cy.get('.todo-add-column-tile-menu-form > #column-title').type('Already watched');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);

        cy.get('#column-2 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'Already watched'
        );
    });
    it('should update column title', function () {
        cy.get('#column-1 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').dblclick();
        cy.wait(2000);

        cy.get('#update-column-title').clear().type('Watching {enter}');
        cy.wait(2000);

        cy.get('#column-1 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contain.text',
            'Watching'
        );
    });
    it('should add cards', function () {
        cy.get('#column-0 > .mat-card > .mat-card-content > .todo-board-cards > #add-card').click();
        cy.wait(2000);

        cy.get('.todo-add-card-tile-menu-form > #card-title').type("The Queen's Gambit");
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);

        cy.get('#column-1 > .mat-card > .mat-card-content > .todo-board-cards > #add-card').click();
        cy.wait(2000);

        cy.get('.todo-add-card-tile-menu-form > #card-title').type('Supernatural');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);

        cy.get('#column-2 > .mat-card > .mat-card-content > .todo-board-cards > #add-card').click();
        cy.wait(2000);

        cy.get('.todo-add-card-tile-menu-form > #card-title').type('Breaking Bad');
        cy.wait(2000);

        cy.get('#submit').click();
        cy.wait(5000);
    });
    it('should move columns', function () {
        cy.get(
            '#column-0 > :nth-child(1) > :nth-child(1) > app-editable > .todo-board-column-title-view-mode > #open-column-menu-action'
        ).click();
        cy.wait(2000);

        cy.get('#move-right').click();
        cy.wait(2000);

        cy.get('#column-0 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'Watching'
        );

        cy.get('#column-1 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'To watch'
        );

        cy.get(
            '#column-1 > :nth-child(1) > :nth-child(1) > app-editable > .todo-board-column-title-view-mode > #open-column-menu-action'
        ).click();
        cy.wait(2000);

        cy.get('#move-left').click();
        cy.wait(2000);

        cy.get('#column-0 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'To watch'
        );

        cy.get('#column-1 > .mat-card > .mat-card-title > app-editable > .todo-board-column-title-view-mode > #column-title').should(
            'contains.text',
            'Watching'
        );
    });
});
