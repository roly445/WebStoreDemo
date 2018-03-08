export class CardPayment {

    private integration:Integration;
    private onShippingAddressSelectedUserFunc:(shippingAddress:PaymentAddress) => Promise<PaymentShippingOption[]>;
    private originalTotal: PaymentItem;
    private paymentShippingOptions:PaymentShippingOption[]
    private shippingInfo: ShippingInfo;

    public canMakePayments(): boolean { return this.integration.cardPaymentEnabled; }
    constructor(private identifyingToken: string) {
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

    public startPayment(paymentInfo: PaymentInfo,
        cancel?: (reason:any)=>void,
        shippingInfo?: ShippingInfo,
        additionalOptions?:AdditionalOptions
    ) {
        const contextThis = this;
        const supportedPaymentMethods:PaymentMethodData[] = [{
            supportedMethods: 'basic-card',
            data: {
                supportedNetworks: contextThis.integration.schemes,
              },
        }];
        this.originalTotal = {
            label: paymentInfo.totalLabel ? paymentInfo.totalLabel : 'Total',
            amount:{
                currency: this.integration.currency,
                value: paymentInfo.amount.toString()
            }
        }
        let paymentDetails:PaymentDetails = {
            total: this.originalTotal
        };
        let options: PaymentOptions = {};
        if (shippingInfo) {
            options.requestShipping = true;
            if(shippingInfo.useSubTotal) {
                paymentDetails.displayItems = [];
                paymentDetails.displayItems.push({
                    label:  shippingInfo.subTotalLabel ? shippingInfo.subTotalLabel : 'Sub Total',
                    amount: {
                        value: this.originalTotal.amount.value,
                        currency:  this.integration.currency
                    }
                })
                paymentDetails.displayItems.push({
                    label: shippingInfo.deliveryLabel ? shippingInfo.deliveryLabel : 'Delivery',
                    amount: {
                        value: '0',
                        currency:  this.integration.currency
                    }
                })
            }
        }
        if (additionalOptions) {
            options.requestPayerEmail = additionalOptions.requestPayerEmail;
            options.requestPayerName = additionalOptions.requestPayerName;    
            options.requestPayerPhone = additionalOptions.requestPayerPhone;
        }
        
        var paymentRequest = new PaymentRequest(
            supportedPaymentMethods,
            paymentDetails,
            options
        );

        if (shippingInfo) {
            this.shippingInfo = shippingInfo;
            this.onShippingAddressSelectedUserFunc = shippingInfo.onShippingAddressSelected;
            paymentRequest.addEventListener('shippingaddresschange', (event:PaymentRequestUpdateEvent) => { 
                contextThis.onShippingAddressChange(event);
            });
            paymentRequest.addEventListener('shippingoptionchange', (event:PaymentRequestUpdateEvent) => {
                contextThis.onShippingOptionChange(event);
            });            
        }
        
        

        return paymentRequest
            .show()
            .then((paymentResponse: PaymentResponse) => {
                
                return fetch('api/gateway/make-card-payment', {
                    method: 'post',
                    body: JSON.stringify({
                        identifingToken: contextThis.identifyingToken,
                        details: paymentResponse.details
                    }),
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                })
                .then((serverResponse: Response) => {
                    return serverResponse.json()
                    .then((gatewayResponse:GatewayResponse) => {
                        if (gatewayResponse.success) {
                            return paymentResponse.complete('success')
                            .then(value => {
                                var data:PaymentResult = {
                                    success: gatewayResponse.success,
                                    statusCode: gatewayResponse.statusCode,
                                    statusMessage: gatewayResponse.statusMessage,
                                    methodName: paymentResponse.methodName,
                                    payerEmail: paymentResponse.payerEmail,
                                    payerName: paymentResponse.payerName,
                                    payerPhone: paymentResponse.payerPhone,
                                    shippingAddress: paymentResponse.shippingAddress,
                                    shippingOption: paymentResponse.shippingOption
                                };
                                return data;
                            });
                        } else {
                            return paymentResponse.complete('success')
                            .then(value => {
                                var data:PaymentResult = {
                                    success: gatewayResponse.success,
                                    statusCode: gatewayResponse.statusCode,
                                    statusMessage: gatewayResponse.statusMessage,
                                    methodName: paymentResponse.methodName
                                };
                                return data;
                            });
                        }
                    })
                })
            })        
    }

    private onShippingAddressChange(event:PaymentRequestUpdateEvent) {
        const paymentRequest:PaymentRequest = event.target as PaymentRequest;
        const contextThis = this;
        event.updateWith(
            this.onShippingAddressSelectedUserFunc(paymentRequest.shippingAddress)
                .then((paymentShippingOptions: PaymentShippingOption[])=> {
                    contextThis.paymentShippingOptions = paymentShippingOptions;
                    let paymentItems:PaymentItem[] = [];
                    if(contextThis.shippingInfo.useSubTotal) {
                        
                        paymentItems.push({
                            label:  contextThis.shippingInfo.subTotalLabel ? contextThis.shippingInfo.subTotalLabel : 'Sub Total',
                            amount: {
                                value: this.originalTotal.amount.value,
                                currency:  this.integration.currency
                            }
                        })
                        paymentItems.push({
                            label: contextThis.shippingInfo.deliveryLabel ? contextThis.shippingInfo.deliveryLabel : 'Delivery',
                            amount: {
                                value: '0',
                                currency:  this.integration.currency
                            }
                        })
                    }

                    return {
                        total: contextThis.originalTotal,
                        displayItems: paymentItems,
                        shippingOptions: paymentShippingOptions
                    };
                })
            );
    }

    private onShippingOptionChange(event:PaymentRequestUpdateEvent) {
        const paymentRequest:PaymentRequest = event.target as PaymentRequest;
        const contextThis = this;
        const selectedId = paymentRequest.shippingOption;
        
        let shippingCost:number = 0;
        contextThis.paymentShippingOptions.forEach((option) => {
            if (option.id === selectedId) {
                option.selected = true;
                shippingCost = parseFloat(option.amount.value);
            } else {
                option.selected = false;
            }            
        });
      
        var newTotal = JSON.parse(JSON.stringify(this.originalTotal));
        newTotal.amount.value = (parseFloat(newTotal.amount.value) + shippingCost).toString();
        let paymentItems:PaymentItem[] = [];
                    if(contextThis.shippingInfo.useSubTotal) {
                        
                        paymentItems.push({
                            label:  contextThis.shippingInfo.subTotalLabel ? contextThis.shippingInfo.subTotalLabel : 'Sub Total',
                            amount: {
                                value: this.originalTotal.amount.value,
                                currency:  this.integration.currency
                            }
                        })
                        paymentItems.push({
                            label: contextThis.shippingInfo.deliveryLabel ? contextThis.shippingInfo.deliveryLabel : 'Delivery',
                            amount: {
                                value: shippingCost.toString(),
                                currency:  this.integration.currency
                            }
                        })
                    }

        event.updateWith(Promise.resolve( {
           total: newTotal,
           displayItems: paymentItems,
           shippingOptions: contextThis.paymentShippingOptions
        }));
    }
}

interface PaymentInfo {
    amount: number;
    totalLabel?: string    
}
interface ShippingInfo {
    onShippingAddressSelected:(shippingAddress:PaymentAddress) => Promise<PaymentShippingOption[]>;
    useSubTotal?: boolean
    subTotalLabel?: string
    deliveryLabel?: string
}

interface AdditionalOptions {
    requestPayerEmail?:boolean;
    requestPayerName?:boolean;
    requestPayerPhone?:boolean;
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

interface GatewayResponse {
    success: boolean;
    statusCode: number;
    statusMessage: string;
}