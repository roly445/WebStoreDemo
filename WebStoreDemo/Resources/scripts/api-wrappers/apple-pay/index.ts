export class ApplePay {
    private integration:Integration;
    private session: ApplePaySession;
    private onShippingContactSelectedUserFunc:(address:ApplePayJS.ApplePayPaymentContact) => Promise<ApplePayJS.ApplePayShippingMethod[]>;
    constructor (private identifyingToken: string) {
        var contextThis = this;
        fetch('https://localhost:44304/integration', {
            method: 'POST',
            body: JSON.stringify({identifyingToken: identifyingToken}),
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            }
            }).then(res => {
                res.json().then((integration:Integration) =>  {
                    contextThis.integration = integration
            })            
        });
    }

    startPayment(): Promise<PaymentResult> {
        const contextThis = this;
        var promise = new Promise<PaymentResult>((resolve, reject) => {
            this.session = new ApplePaySession(1, {
                total: {
                    amount: '0',
                    label: 'Total',
                },
                countryCode: '',
                currencyCode: '',
                merchantCapabilities: [],
                supportedNetworks:[],
            })
            this.session.addEventListener('validatemerchant', (event:any) => {
                contextThis.onValidateMerchant(event, reject);
            });
            this.session.addEventListener('paymentauthorized', (event:any) => {
                contextThis.onPaymentAuthorized(event, resolve, reject);
            })
            this.session.addEventListener('cancel', (event:any) => {
                contextThis.onCancel(event, reject);
            })
            this.session.addEventListener('shippingcontactselected', (event:any) => {
                contextThis.onShippingContactSelected(event);
            });
            this.session.addEventListener('shippingmethodselected', (event:any)=> {
                contextThis.onShippingMethodSelected(event);
            });
        });

        return promise;
        
        
        
        

        
    }

    private onValidateMerchant (event: ApplePayJS.ApplePayValidateMerchantEvent, reject:any) {
        const contextThis = this;
        const validationUrl = event.validationURL
        fetch('https://localhost:44304/session/apple-pay', {
            method: 'POST',
            body: JSON.stringify({validationUrl: validationUrl}),
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            }
        }).then((response) => {
            contextThis.session.completeMerchantValidation(response);
        }).catch((reason: any) => {
            reject()
        })
    }

    private onPaymentAuthorized (event:ApplePayJS.ApplePayPaymentAuthorizedEvent, resolve, reject) {
        // var gateway = new Gateway();
        // gateway.processApplePayment(event.payment.token, this.merchantData);
        
        
    }
    private onCancel(event:ApplePayJS.Event, reject) {
        reject();
    }

    private onShippingContactSelected (event:ApplePayJS.ApplePayShippingContactSelectedEvent) {
        const contextThis = this;
        contextThis.onShippingContactSelectedUserFunc(event.shippingContact)
            .then((shippingMethods: ApplePayJS.ApplePayShippingMethod[])=> {
                contextThis.session.completeShippingContactSelection(
                ApplePaySession.STATUS_SUCCESS,
                shippingMethods,
                {
                  label: '',
                  amount: '0'  
                },
                []
            )
        })        
    }

    private onShippingMethodSelected (event:ApplePayJS.ApplePayShippingMethodSelectedEvent) {
        event.shippingMethod
        this.session.completeShippingMethodSelection(
            ApplePaySession.STATUS_SUCCESS,
            {
                label: '',
                amount: '0'  
              },
              []

        )
    }
}

interface Integration {
    applePayEnabled: boolean;
    googlePayEnabled: boolean;
    cardPaymentEnabled: boolean;
    currency: string;
    schemes: string[];
    methods: string[];
}

interface PaymentResult {
    success: boolean;
    statusCode: number;
    statusMessage: string;
    methodName: string;
    payerEmail?: string;
    payerName?: string;
    payerPhone?: string;
    shippingAddress?: PaymentAddress;
    shippingOption?: string;
}