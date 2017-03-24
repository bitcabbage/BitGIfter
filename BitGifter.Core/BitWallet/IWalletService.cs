using BitGifter.Core.BitWallet.Messages;

namespace BitGifter.Core.BitWallet
{
    public interface IWalletService
    {
        WalletResponse CreateWallet(WalletRequest request);
        PaymentResponse MakePayment(PaymentRequest request);
    }
}