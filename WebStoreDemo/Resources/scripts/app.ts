class App {
    
    constructor() {
        if (document.readyState !== 'loading') {
            this.init();
        } else {
            document.addEventListener('DOMContentLoaded', e => this.init());
        }        
    }

    init(): void {
        var body = document.querySelector('body') as HTMLElement;
        
        var pageClass = body.dataset['pageClass'];
        if (pageClass) {
            (<any>window).webStoreDemo = (<any>window).webStoreDemo || {};
            if ((<any>window).webStoreDemo[pageClass]) {
                let pageObject = new (<any>window).webStoreDemo[pageClass];
            } else {
                console.log(pageClass + ' does not exist');
            }
        }
    }
}

let app = new App();