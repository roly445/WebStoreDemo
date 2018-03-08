import {CardPayment} from '../api-wrappers/card-payment'

export class Basket {
    $pay: HTMLButtonElement;
    
    cardPayment: CardPayment;
    constructor() {
      this.cardPayment = new CardPayment('');
      this.$pay = document.querySelector('#pay') as HTMLButtonElement;
      this.$pay.addEventListener('click', (e) => this.makeCardPayment(e))

    }

    getShippingDetails (shippingAddress:PaymentAddress):Promise<PaymentShippingOption[]> {
      return fetch('api/fake-shipping-provider', {
            method: 'POST',
            body: JSON.stringify({postalcode: shippingAddress.postalCode}),
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json'
            }
        }).then(res => {
            return res.json()
            .then((shippingMethodResponse:ShippingMethodResponse[]) =>  {
              var paymentShippingOptions: PaymentShippingOption[] = [];

              shippingMethodResponse.forEach(element => {
                paymentShippingOptions.push({
                  id: element.id,
                  label: element.label,
                  amount: {
                    currency: 'GBP',
                    value: element.value.toFixed(2).toString()
                  }
                })
              });
                return paymentShippingOptions;
            })            
        });
    }

    private makeCardPayment(e:MouseEvent) {
        e.preventDefault();
        
        let $totalAmount: HTMLSpanElement = document.querySelector('#total-amount');

        this.cardPayment.startPayment({
            amount: parseFloat($totalAmount.innerText),            
          },
          null, {
            onShippingAddressSelected:  this.getShippingDetails,
            useSubTotal:true
          },
          {
            requestPayerEmail: true
          }
         
        );

        // this.$totalAmount = document.querySelector('#total-amount');


        // const supportedPaymentMethods = [
        //     {
        //       supportedMethods: 'basic-card',
        //     }
        //   ];
        //   const paymentDetails = {
        //     total: {
        //       label: 'Total',
        //       amount:{
        //         currency: 'GBP',
        //         value: this.$totalAmount.innerText
        //       }
        //     }
        //   };
        //   // Options isn't required.
        //   const options = {
        //     requestShipping: true
        //   };
          
        //   var paymentRequest = new PaymentRequest(
        //     supportedPaymentMethods,
        //     paymentDetails,
        //     options
        //   );
        
        //   paymentRequest.addEventListener('shippingaddresschange', (event:any) => {
        //     const paymentDetails = {
        //       total: {
        //         label: 'Total',
        //         amount: {
        //           currency: 'USD',
        //           value: 10,
        //         },
        //       }              
        //     };
        //     event.updateWith(paymentDetails);
        //   });

        //     paymentRequest.show()
        //       .then((response: PaymentResponse) => {
        //         fetch('api/gateway/make-card-payment', {
        //           method: 'post',
        //           body: JSON.stringify(response),
        //           headers: {
        //             'Accept': 'application/json',
        //             'Content-Type': 'application/json'
        //           }
        //         }).then(res => {
        //           response.complete('success');
        //         });
        //         debugger;
                
        //       }).catch(err => {
        //         debugger;
        //       })

    }
}

interface ShippingMethodResponse
{
  id:string;
  value: number;
  label: string;
}
