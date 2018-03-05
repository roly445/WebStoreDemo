export class Basket {
    $pay: HTMLButtonElement;
    $totalAmount: HTMLSpanElement;
    constructor() {
        this.$pay = document.querySelector('#pay') as HTMLButtonElement;
        this.$pay.addEventListener('click', (e) => this.cardPayment(e))
    }

    private cardPayment(e:MouseEvent) {
        e.preventDefault();

        this.$totalAmount = document.querySelector('#total-amount');


        const supportedPaymentMethods = [
            {
              supportedMethods: 'basic-card',
            }
          ];
          const paymentDetails = {
            total: {
              label: 'Total',
              amount:{
                currency: 'GBP',
                value: this.$totalAmount.innerText
              }
            }
          };
          // Options isn't required.
          const options = {
            requestShipping: true
          };
          
          var paymentRequest = new PaymentRequest(
            supportedPaymentMethods,
            paymentDetails,
            options
          );
        
          paymentRequest.addEventListener('shippingaddresschange', (event:any) => {
            const paymentDetails = {
              total: {
                label: 'Total',
                amount: {
                  currency: 'USD',
                  value: 10,
                },
              }              
            };
            event.updateWith(paymentDetails);
          });

            paymentRequest.show()
              .then((response: PaymentResponse) => {
                fetch('api/gateway/make-card-payment', {
                  method: 'post',
                  body: JSON.stringify(response),
                  headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                  }
                }).then(res => {
                  response.complete('success');
                });
                debugger;
                
              }).catch(err => {
                debugger;
              })

    }
}
